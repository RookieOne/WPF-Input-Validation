using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CommonLibrary.Wpf;

namespace EnterpriseLibraryExample.Foundation
{
    public class EntLibViewModel : ViewModel, IDataErrorInfo
    {
        public string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }

        public string Error
        {
            get { return null; }
        }
    }
}
