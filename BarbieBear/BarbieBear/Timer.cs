using System;

namespace BarbieBear
{
    public class Timer
    {
        public int Count { get; protected set; }
        public string Name { get; set; }
        public int Limit { get; set; }
        public bool ShouldRepeat { get; set; }
        public Action OnDing { get; set; }

        public bool Finished { get { return Stopped && Count == Limit && !ShouldRepeat; } }

        public bool Stopped { get; set; }

        public Timer(string name, int limit, Action onDing) : this(name, limit, false, true, onDing)
        {
        }

        public Timer(string name, int limit, bool startImmediately, Action onDing) : this(name, limit, false, startImmediately, onDing)
        {
        }

        public Timer(string name, int limit, bool isRepeating, bool startImmediately, Action onDing)
        {
            this.Name = name;
            this.Limit = limit;
            this.ShouldRepeat = isRepeating;
            this.OnDing = onDing;

            if (startImmediately)
                this.Start();
            else
                this.Stop();
        }

        private void Stop()
        {
            this.Stopped = true;
        }

        private void Start()
        {
            this.Stopped = false;
        }


        public void Update()
        {
            if (!this.Stopped)
            {
                this.Count++;

                if (this.Ding() && this.OnDing != null)
                {
                    this.OnDing();
                    if (!this.ShouldRepeat)
                    {
                        this.Stop();
                    }
                }
            }
        }

        public bool Ding()
        {
            return this.Count - this.Limit == 0;
        }
    }
}
