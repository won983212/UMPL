using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MathCalc
{
    public class RemovableElement<T> : INotifyPropertyChanged where T : RemovableElement<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DispatcherTimer removalTimer = null;
        private ObservableCollection<T> fromRemove = null;
        private bool _removing = false;
        public bool Removing
        {
            get
            {
                return _removing;
            }
            private set
            {
                _removing = value;
                OnPropertyChanged("Removing");
            }
        }

        public void MarkAsRemove(ObservableCollection<T> fromList)
        {
            if (removalTimer == null)
            {
                fromRemove = fromList;
                Removing = true;

                removalTimer = new DispatcherTimer();
                removalTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                removalTimer.Tick += RemovalTimer_Tick;
                removalTimer.Start();
            }
        }

        private void RemovalTimer_Tick(object sender, EventArgs e)
        {
            fromRemove.Remove((T) this);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
