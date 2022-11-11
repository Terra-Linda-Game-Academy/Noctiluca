using System;

namespace Input {
	[Serializable]
	public abstract class InputMiddleware<T, E> {
		private   E _events;
		protected E Events {
			get => _events;
			set {
				_events = value;
				if (_eventsSubscribed) DisposeSubscriptions();
				EventSubscriptions();
				_eventsSubscribed = true;
			}
		}

		private bool _eventsSubscribed;

		public abstract void TransformInput(ref T inputData, ref E events);

		protected abstract void EventSubscriptions();
		protected abstract void DisposeSubscriptions();
	}
}