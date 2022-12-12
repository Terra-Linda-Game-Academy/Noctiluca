namespace Input.Middleware {
	public class DumbStupidMiddleware<T, D> : InputMiddleware<T, D> {
		public override void TransformInput(ref T inputData) { throw new System.NotImplementedException(); }
		public override void Init()                          { throw new System.NotImplementedException(); }
	}
}