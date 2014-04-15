
namespace WickedFlame.InjectionMap.Test.Mock
{
    public interface IConstructorParameter
    {
        int ID { get; }
    }

    public class ConstructorParameter : IConstructorParameter
    {
        public int ID
        {
            get
            {
                return 2;
            }
        }
    }

    public interface IConstructorInjectionMock
    {
        int ID { get; }
    }

    public class ConstructorInjectionMock : IConstructorInjectionMock
    {
        public ConstructorInjectionMock()
        {
            ID = 1;
        }

        [InjectionConstructor]
        public ConstructorInjectionMock(IConstructorParameter parameter)
        {
            ID = parameter.ID;
        }

        public int ID { get; private set; }
    }

    public interface ISecondConstructorInjectionMock : IConstructorInjectionMock
    {
    }

    public class SecondConstructorInjectionMock : ISecondConstructorInjectionMock
    {
        public SecondConstructorInjectionMock()
        {
            ID = 1;
        }

        [InjectionConstructor]
        public SecondConstructorInjectionMock(IConstructorParameter parameter, IConstructorInjectionMock second)
        {
            ID = parameter.ID + second.ID;
        }

        public int ID { get; private set; }
    }

    public interface IThirdConstructorInjectionMock : ISecondConstructorInjectionMock
    {
    }

    public class ThirdConstructorInjectionMock : IThirdConstructorInjectionMock
    {
        public ThirdConstructorInjectionMock()
        {
            ID = 1;
        }

        [InjectionConstructor]
        public ThirdConstructorInjectionMock(IConstructorParameter parameter, IConstructorInjectionMock second, ISecondConstructorInjectionMock third)
        {
            ID = parameter.ID + second.ID + third.ID;
        }

        public int ID { get; private set; }
    }
}
