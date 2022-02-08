using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TegningHenter.Commands;

namespace TegningHenter.ViewModels
{
    public class MainViewModel
    {
        public string Address { get; set; }
        public ICommand GetAddressCommand { get; }

        public MainViewModel()
        {
            GetAddressCommand = new GetAddressCommand(this);
        }
    }
}
