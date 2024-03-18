namespace LCPC.Domain.Commands
{
	public class CreatUserCommand:IRequest<ReturnResult>
	{
		public string Name { get; set; }
		public string Pass { get; set; }
		public string Address { get; set; }
		public string Tel { get; set; }
		public bool Enable { get; set; }
		public string Remark { get; set; }
	}
}

