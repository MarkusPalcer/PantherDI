 
  
  

// Generated code. See DelegateFactoryCreators.tt for code generation

using System;

namespace PantherDI.Registry.Registration.Factory
{

	public partial class DelegateFactory 
	{
		
		public static DelegateFactory Create<TIn1, T>(Func<TIn1, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0]), contracts, new[] { typeof(TIn1) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, T>(Func<TIn1, TIn2, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1]), contracts, new[] { typeof(TIn1), typeof(TIn2) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, T>(Func<TIn1, TIn2, TIn3, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, T>(Func<TIn1, TIn2, TIn3, TIn4, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4], (TIn6)p[5]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4], (TIn6)p[5], (TIn7)p[6]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4], (TIn6)p[5], (TIn7)p[6], (TIn8)p[7]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4], (TIn6)p[5], (TIn7)p[6], (TIn8)p[7], (TIn9)p[8]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4], (TIn6)p[5], (TIn7)p[6], (TIn8)p[7], (TIn9)p[8], (TIn10)p[9]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4], (TIn6)p[5], (TIn7)p[6], (TIn8)p[7], (TIn9)p[8], (TIn10)p[9], (TIn11)p[10]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4], (TIn6)p[5], (TIn7)p[6], (TIn8)p[7], (TIn9)p[8], (TIn10)p[9], (TIn11)p[10], (TIn12)p[11]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11), typeof(TIn12) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4], (TIn6)p[5], (TIn7)p[6], (TIn8)p[7], (TIn9)p[8], (TIn10)p[9], (TIn11)p[10], (TIn12)p[11], (TIn13)p[12]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11), typeof(TIn12), typeof(TIn13) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4], (TIn6)p[5], (TIn7)p[6], (TIn8)p[7], (TIn9)p[8], (TIn10)p[9], (TIn11)p[10], (TIn12)p[11], (TIn13)p[12], (TIn14)p[13]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11), typeof(TIn12), typeof(TIn13), typeof(TIn14) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4], (TIn6)p[5], (TIn7)p[6], (TIn8)p[7], (TIn9)p[8], (TIn10)p[9], (TIn11)p[10], (TIn12)p[11], (TIn13)p[12], (TIn14)p[13], (TIn15)p[14]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11), typeof(TIn12), typeof(TIn13), typeof(TIn14), typeof(TIn15) });
		}

		
		public static DelegateFactory Create<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, TIn16, T>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, TIn16, T> @delegate, params object[] contracts)
		{
			return new DelegateFactory(p => @delegate((TIn1)p[0], (TIn2)p[1], (TIn3)p[2], (TIn4)p[3], (TIn5)p[4], (TIn6)p[5], (TIn7)p[6], (TIn8)p[7], (TIn9)p[8], (TIn10)p[9], (TIn11)p[10], (TIn12)p[11], (TIn13)p[12], (TIn14)p[13], (TIn15)p[14], (TIn16)p[15]), contracts, new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11), typeof(TIn12), typeof(TIn13), typeof(TIn14), typeof(TIn15), typeof(TIn16) });
		}

	}
}