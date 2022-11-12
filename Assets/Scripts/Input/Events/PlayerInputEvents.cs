using System;

namespace Input.Events {
	public struct PlayerInputEvents : IInputEvents<PlayerInputEvents, byte> {
		public event Action<byte> Event0;

		public bool CanCallEvent0(PlayerInputEvents input) { throw new NotImplementedException(); }
	}
}