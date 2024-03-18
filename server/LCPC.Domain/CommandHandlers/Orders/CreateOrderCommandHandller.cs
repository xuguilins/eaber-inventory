using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LCPC.Domain.Notifys;

namespace LCPC.Domain.CommandHandlers
{
    public class CreateOrderCommandHandller : IRequestHandler<CreateOrderCommand, ReturnResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRuleManager _ruleManager;
        private readonly IProdcutRepository _productRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IMediator _mediator;
        private readonly UserHelper _userHelper;
        private readonly ICustomerInfoRepository _customerInfoRepository;
        private readonly ISqlDapper _sqlDapper;
        public CreateOrderCommandHandller(IOrderRepository orderRepository,
            IRuleManager ruleManager, IProdcutRepository prodcutRepository, 
            IUserInfoRepository userInfoRepository,
            IMediator mediator,
            ICustomerInfoRepository customerInfoRepository,
            ISqlDapper sqlDapper,
            UserHelper userHelper)
        {
            _orderRepository = orderRepository;
            _ruleManager = ruleManager;
            _productRepository = prodcutRepository;
            _userInfoRepository = userInfoRepository;
            _mediator = mediator;
            _userHelper = userHelper;
            _customerInfoRepository = customerInfoRepository;
            _sqlDapper = sqlDapper;

        }

        public async Task<ReturnResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new OrderInfo();
            order.OrderCode = await _ruleManager.getNextRuleNumber(RuleType.Order);
            order.OrderTel = request.SellPhone;
            order.OrderTime = request.SellTime;
            order.OrderStatus = OrderStatus.AWAIT;
            order.OrderPay = request.PayName;
            order.Remark = request.Remark;
            order.CreateUser = _userHelper.LoginName;
            order.ActuailMoney = request.ActuailMoney;
            order.OffsetMoney = request.OffsetMoney;
            order.OrderUser = request.SellUser;
            var numbers = request.Products.Select(x => x.ProductCode).Distinct()
                .ToArray();
            bool isValidate = true;
            string message = string.Empty; 
            order.OrderUserId=  await CreateCustomer(request.SellUser, request.SellPhone);
            #region 循环验证库存
            // 验证产品库存
            var products =
                await _productRepository.GetIncludeProduct(numbers);
            decimal prices = 0.00M;
            for (int i = 0; i < request.Products.Count; i++)
            {
                var item = request.Products[i];
                var product = products.FirstOrDefault(x => x.ProductCode == item.ProductCode);
                if (product == null)
                {
                    message = $"{item.ProductName}不存在";
                    isValidate = false;
                    break;
                }
                if (item.OrderCount > product.InventoryCount)
                {
                    message = $"{product.ProductName}库存不足，剩【{product.InventoryCount}】个";
                    isValidate = false;
                    break;
                }
                var money = item.OrderCount * item.OrderSigle;
                if (money != item.OrderPrice)
                {
                    message = $"{product.ProductName}价格异常";
                    isValidate = false;
                    break;
                }

                prices += money;
                product.InventoryCount -= item.OrderCount;
                await _productRepository.UpdateAsync(product);
                var unitNameValue = await getUnitName(product.UnitId);
                order.OrderInfoDetails.Add(new OrderInfoDetail
                {
                    ProductId = product.Id,
                    ProductCode = product.ProductCode,
                    ProductName = product.ProductName,
                    OrderCount = item.OrderCount,
                    OrderPrice = item.OrderPrice,
                    OrderSigle = item.OrderSigle,
                    CreateUser = _userHelper.LoginName,
                    Remark = item.Remark,
                    UnitName = unitNameValue
                });
            }
            if (!isValidate)
                throw new Exception(message);

            #endregion

            order.OrderMoney = prices;
            if (order.ActuailMoney <= 0)
                order.ActuailMoney = order.OrderMoney;
            await _orderRepository.AddAsync(order);
          
            int result = await _orderRepository.UnitOfWork.SaveChangesAsync();
            if (result > 0)
                await _mediator.Publish(new OrderNotify());
       
            return result > 0
                ? new ReturnResult(true, order.OrderCode, "订单创建成功")
                : new ReturnResult(false, null, "订单创建失败");
        }

        private async Task<string>  CreateCustomer(string name, string tel)
        {
            var model = await _customerInfoRepository
                .FindEntity(d => d.CustomerName.Equals(name)
                                 && d.CreateUser.Equals(_userHelper.LoginName));
            if (model == null)
            {
                var code = await _ruleManager.getNextRuleNumber(RuleType.Customer);
                CustomerInfo customerInfo = new CustomerInfo
                {
                    CustomerCode = code,
                    CustomerName = name,
                    TelNumber = tel,
                    PhoneNumber = tel,
                    CustomerUser = name,
                    CreateUser = _userHelper.LoginName,
                    Enable = true,

                };
                await _customerInfoRepository.AddAsync(customerInfo);
                return customerInfo.Id;
            }

            return model.Id;
        }

        private async Task<string> getUnitName(string id)
        {
            string sql = "select  DicCode from SystemDicInfo  where Id=@Id";
            var obj = await _sqlDapper.ExecuteScalerAsync(sql, new { Id = id });
            return Convert.ToString(obj);
        }
    }
}