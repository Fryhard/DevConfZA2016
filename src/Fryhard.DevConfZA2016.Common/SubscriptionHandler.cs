using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fryhard.DevConfZA2016.Common
{
    public class SubscriptionHandler
    {
        private static Dictionary<string, IDisposable> _SubscriptionStore;

        private static SubscriptionHandler _Instance;

        public static SubscriptionHandler Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SubscriptionHandler();
                }
                return _Instance;
            }
        }

        private SubscriptionHandler()
        {
            _SubscriptionStore = new Dictionary<string, IDisposable>();
        }

        public void AddSubscription (string name, IDisposable sub)
        {
            if (_SubscriptionStore.ContainsKey (name))
            {
                _SubscriptionStore[name] = sub;
            }
            else
            {
                _SubscriptionStore.Add(name, sub);
            }
        }

        public void Unsubscribe (string name)
        {
            if (_SubscriptionStore.ContainsKey(name))
            {
                IDisposable sub = _SubscriptionStore[name];
                if (sub != null)
                {
                    sub.Dispose(); //Unsub from the queue
                    _SubscriptionStore.Remove(name); // Remove this sub from the store
                }
            }
        }

        public bool CheckForSubsctiption (string name)
        {
            return _SubscriptionStore.ContainsKey(name);
        }

    }
}
