using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMC_API;

namespace TMC_VIEW_MODEL
{
    /*
     * 
     * Base class of ViewModel.
     * It allows to acces all TMC data and notifications
     */
    public partial class BaseTMCViewModel : ObservableObject
    {
        [ObservableProperty]
        private TMC_Model _TMC_Model;


        public virtual void OnHide()
        {
            throw new NotImplementedException();
        }
        public virtual void OnShow()
        {
            throw new NotImplementedException();
        }
        public BaseTMCViewModel(TMC_Model TMC_Model)
        {
            this.TMC_Model = TMC_Model;
        }
    }
}
