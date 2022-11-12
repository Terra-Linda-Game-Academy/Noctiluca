using System;

namespace Input.Events {
	public interface IInputEvents<in T, out E0> {
		public event Action<E0> Event0;

		public bool CanCallEvent0(T input);
	}

	public interface IInputEvents<in T, out E0, out E1> {
		public event Action<E0> Event0;
		public event Action<E1> Event1;
		
		public bool CanCallEvent0(T input);
		public bool CanCallEvent1(T input);
	}

	public interface IInputEvents<in T, out E0, out E1, out E2> {
		public event Action<E0> Event0;
		public event Action<E1> Event1;
		public event Action<E2> Event2;
		
		public bool CanCallEvent0(T input);
		public bool CanCallEvent1(T input);
		public bool CanCallEvent2(T input);
	}

	public interface IInputEvents<in T, out E0, out E1, out E2, out E3> {
		public event Action<E0> Event0;
		public event Action<E1> Event1;
		public event Action<E2> Event2;
		public event Action<E3> Event3;
		
		public bool CanCallEvent0(T input);
		public bool CanCallEvent1(T input);
		public bool CanCallEvent2(T input);
		public bool CanCallEvent3(T input);
	}

	public interface IInputEvents<in T, out E0, out E1, out E2, out E3, out E4> {
		public event Action<E0> Event0;
		public event Action<E1> Event1;
		public event Action<E2> Event2;
		public event Action<E3> Event3;
		public event Action<E4> Event4;
		
		public bool CanCallEvent0(T input);
		public bool CanCallEvent1(T input);
		public bool CanCallEvent2(T input);
		public bool CanCallEvent3(T input);
		public bool CanCallEvent4(T input);
	}

	public interface IInputEvents<in T, out E0, out E1, out E2, out E3, out E4, out E5> {
		public event Action<E0> Event0;
		public event Action<E1> Event1;
		public event Action<E2> Event2;
		public event Action<E3> Event3;
		public event Action<E4> Event4;
		public event Action<E5> Event5;
		
		public bool CanCallEvent0(T input);
		public bool CanCallEvent1(T input);
		public bool CanCallEvent2(T input);
		public bool CanCallEvent3(T input);
		public bool CanCallEvent4(T input);
		public bool CanCallEvent5(T input);
	}

	public interface IInputEvents<in T, out E0, out E1, out E2, out E3, out E4, out E5, out E6> {
		public event Action<E0> Event0;
		public event Action<E1> Event1;
		public event Action<E2> Event2;
		public event Action<E3> Event3;
		public event Action<E4> Event4;
		public event Action<E5> Event5;
		public event Action<E6> Event6;
		
		public bool CanCallEvent0(T input);
		public bool CanCallEvent1(T input);
		public bool CanCallEvent2(T input);
		public bool CanCallEvent3(T input);
		public bool CanCallEvent4(T input);
		public bool CanCallEvent5(T input);
		public bool CanCallEvent6(T input);
	}

	public interface IInputEvents<in T, out E0, out E1, out E2, out E3, out E4, out E5, out E6, out E7> {
		public event Action<E0> Event0;
		public event Action<E1> Event1;
		public event Action<E2> Event2;
		public event Action<E3> Event3;
		public event Action<E4> Event4;
		public event Action<E5> Event5;
		public event Action<E6> Event6;
		public event Action<E7> Event7;
		
		public bool CanCallEvent0(T input);
		public bool CanCallEvent1(T input);
		public bool CanCallEvent2(T input);
		public bool CanCallEvent3(T input);
		public bool CanCallEvent4(T input);
		public bool CanCallEvent5(T input);
		public bool CanCallEvent6(T input);
		public bool CanCallEvent7(T input);
	}
}