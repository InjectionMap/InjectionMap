using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WickedFlame.InjectionMap.Test.Mock
{
    public interface IConstructorArgumentMock
    {
        string ID { get; }
    }

    public class ConstructorArgumentMock : IConstructorArgumentMock
    {
        public ConstructorArgumentMock(int id, string message)
        {
            ID = string.Format("{0} {1}", message, id);
        }

        public string ID { get; private set; }
    }
}
