namespace Toub.Sound.Midi
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class MidiEventCollection : ICollection, IEnumerable, ICloneable
    {
        private ArrayList _events;

        public MidiEventCollection()
        {
            this._events = new ArrayList();
        }

        public MidiEventCollection(MidiEvent[] events)
        {
            if (events == null)
            {
                throw new ArgumentNullException("events");
            }
            this._events = new ArrayList(events.Length);
            foreach (MidiEvent event2 in events)
            {
                this._events.Add(event2);
            }
        }

        public MidiEventCollection(MidiEventCollection c)
        {
            if (c == null)
            {
                throw new ArgumentNullException("c");
            }
            this._events = new ArrayList(c);
        }

        public virtual int Add(MidiEvent message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            return this._events.Add(message);
        }

        public virtual int Add(MidiEventCollection messages)
        {
            if (messages == null)
            {
                throw new ArgumentNullException("messages");
            }
            if (messages.Count == 0)
            {
                return -1;
            }
            int count = this._events.Count;
            this._events.AddRange(messages);
            return count;
        }

        public virtual MidiEventCollection Clone()
        {
            return new MidiEventCollection(this);
        }

        public virtual bool Contains(MidiEvent message)
        {
            return this._events.Contains(message);
        }

        internal void ConvertDeltasToTotals()
        {
            long deltaTime = this[0].DeltaTime;
            for (int i = 1; i < this.Count; i++)
            {
                deltaTime += this[i].DeltaTime;
                this[i].DeltaTime = deltaTime;
            }
        }

        internal void ConvertTotalsToDeltas()
        {
            long num = 0L;
            for (int i = 0; i < this.Count; i++)
            {
                long deltaTime = this[i].DeltaTime;
                MidiEvent event1 = this[i];
                event1.DeltaTime -= num;
                num = deltaTime;
            }
        }

        public virtual void CopyTo(MidiEvent[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        public virtual IEnumerator GetEnumerator()
        {
            return this._events.GetEnumerator();
        }

        public virtual void Insert(int index, MidiEvent message)
        {
            this._events.Insert(index, message);
        }

        public virtual void Remove(MidiEvent message)
        {
            this._events.Remove(message);
        }

        internal void SortByTime()
        {
            this._events.Sort(new EventComparer());
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this._events.CopyTo(array, index);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public virtual int Count
        {
            get
            {
                return this._events.Count;
            }
        }

        public virtual bool IsSynchronized
        {
            get
            {
                return this._events.IsSynchronized;
            }
        }

        public virtual MidiEvent this[int index]
        {
            get
            {
                return (MidiEvent) this._events[index];
            }
            set
            {
                this._events[index] = value;
            }
        }

        public virtual object SyncRoot
        {
            get
            {
                return this._events.SyncRoot;
            }
        }

        protected class EventComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                MidiEvent event2 = x as MidiEvent;
                MidiEvent event3 = y as MidiEvent;
                if (event2 == null)
                {
                    throw new ArgumentNullException("x");
                }
                if (event3 == null)
                {
                    throw new ArgumentNullException("y");
                }
                return event2.DeltaTime.CompareTo(event3.DeltaTime);
            }
        }
    }
}

