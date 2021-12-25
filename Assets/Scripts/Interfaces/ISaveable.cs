using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface ISaveable
{
    public event Action SaveState;
    public event Action ResetToSavedState;
    public event Action Reset;
}
