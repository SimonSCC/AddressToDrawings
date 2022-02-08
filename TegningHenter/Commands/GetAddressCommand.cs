using Commands;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TegningHenter.ViewModels;

namespace TegningHenter.Commands
{
    public class GetAddressCommand : AsyncCommandBase
    {
        public GetAddressCommand(MainViewModel vm)
        {
            Vm = vm;
        }

        public MainViewModel Vm { get; }

        public override async Task ExecuteAsync(object parameter)
        {
            MainScraperFlow flow = new();
            await flow.TestDownloadFile();

            //await flow.GetDefaultDrawings(Vm.Address, 2);
        }
    }
}
