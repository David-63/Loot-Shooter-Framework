namespace Dave6.LootShooter.Foundation.State
{
    /// <summary>
    /// 최소 상태 실행 단위
    /// 아무 정책 없고, lifecycle 만 제공
    /// </summary>
    public abstract class BaseState<TOwner> : IState
    {
        protected TOwner _Controller;

        public BaseState(TOwner controller) => _Controller = controller;
        public virtual void OnEnter() { }
        public virtual void OnExit() { }

        // Unity LifeCycle
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }

        /// <summary>
        /// 기본적으로 이 상태가 외부에 의해 끊길 수 있는가
        /// </summary>
        public virtual bool CanBeOverridden() => true;

        /// <summary>
        /// 특정 상태만 예외적으로 허용 (기본은 모두 허용)
        /// </summary>
        public virtual bool CanOverrideBy(IState next) => false;

        /// <summary>
        /// 내부적으로 자연스럽게 종료 가능한 상태인가
        /// (예: 애니메이션 끝, 타이머 종료)
        /// </summary>
        public virtual bool CanExit() => true;
    }
}