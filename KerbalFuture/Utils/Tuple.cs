namespace KerbalFuture.Utils
{
    public class Tuple<T1, T2>
    {
        public Tuple(T1 item1, T2 item2)
        {
            this.item1 = item1;
            this.item2 = item2;
        }
        
        public T1 item1 { get; }
        public T2 item2 { get; }
    }

    public class Tuple<T1, T2, T3> : Tuple<T1, T2>
    {
        public Tuple(T1 item1, T2 item2, T3 item3) : base(item1, item2)
        {
            this.item3 = item3;
        }
        
        public T3 item3 { get; }
    }

    public class Tuple<T1, T2, T3, T4> : Tuple<T1, T2, T3>
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4) : base(item1, item2, item3)
        {
            this.item4 = item4;
        }
        
        public T4 item4 { get; }
    }
	
	public class Tuple<T1, T2, T3, T4, T5> : Tuple<T1, T2, T3, T4>
	{
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) : base(item1, item2, item3, item4)
        {
            this.item5 = item5;
        }
        
        public T5 item5 { get; }
	}
	
	public class Tuple<T1, T2, T3, T4, T5, T6> : Tuple<T1, T2, T3, T4, T5>
	{
		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) : base(item1, item2, item3, item4, item5)
        {
            this.item6 = item6;
        }
        
        public T6 item6 { get; }
	}
}
