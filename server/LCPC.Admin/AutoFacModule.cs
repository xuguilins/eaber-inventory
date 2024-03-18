using System.Reflection;
using Autofac;
using FluentValidation;
using LCPC.Domain;
using LCPC.Domain.IRepositories;
using LCPC.Infrastructure.Repositories;
using Module = Autofac.Module;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using LCPC.Domain.Validates;
namespace LCPC.Admin;

public class AutoFacModule:Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assbly = Assembly.Load("LCPC.Domain");
        var types = assbly.GetTypes();
        var validateList = types.Where(x => 
                typeof(IValidator).IsAssignableFrom(x) &&
                x.IsClass 
                )
            .ToArray();
        
        #region 注册验证器
        builder.RegisterTypes(validateList).AsImplementedInterfaces().InstancePerLifetimeScope();
        #endregion

        #region 注册查询服务

        var queryList = types.Where(x =>
            typeof(IScopeDependecy).IsAssignableFrom(x) &&
            x.IsClass && !x.IsAbstract
        ).ToList();
        queryList.ForEach(query =>
        {
            var intres = query.GetInterfaces()
                .FirstOrDefault(x => x!= typeof(IScopeDependecy));
            if (intres != null)
            {
                builder.RegisterType(query).As(intres).InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType(query).InstancePerLifetimeScope();
            }
               
        });

        #endregion

        #region 注册仓储
         builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
         var repAssbly =Assembly.Load("LCPC.Infrastructure").GetTypes();
         var repList = repAssbly.Where(x=>x.Name.EndsWith("Repository"))
         .ToList();
         repList.ForEach(rep=>{
             var inter =  rep.GetInterfaces();
             if( inter.Length>0 && inter.Length>=2){
                 builder.RegisterType(rep).As(inter[1]).InstancePerLifetimeScope();
             }
         });
        #endregion 
       
        #region 注册中介
          var configuration = MediatRConfigurationBuilder
          .Create(assbly)
          .WithAllOpenGenericHandlerTypesRegistered()
          .WithCustomPipelineBehavior (typeof(IValidateBeforeRequest<,>))
            .Build();
       builder.RegisterMediatR(configuration);
       
        #endregion
   
    }
}