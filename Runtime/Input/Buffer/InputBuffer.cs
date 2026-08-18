namespace Dave6.LootShooter.Input
{
    public sealed class InputBuffer
    {
        readonly float _Duration;
        float _RemainingTime;
        public bool HasInput => _RemainingTime > 0f;

        public InputBuffer(float duration) => _Duration = duration;

        public void Push() => _RemainingTime = _Duration;
        public void OnUpdate(float deltaTime)
        {
            if (_RemainingTime <= 0f) return;
            _RemainingTime -= deltaTime;
        }
        public bool Consume()
        {
            if (!HasInput)
                return false;

            _RemainingTime = 0f;
            return true;
        }

        public void Clear() => _RemainingTime = 0f;
    }

    // 나중에 타이머 클래스 구성
    // DDD는 언제?
}