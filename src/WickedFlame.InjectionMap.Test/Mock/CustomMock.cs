
namespace WickedFlame.InjectionMap.Test.Mock
{
    public interface ICustomMock
    {
        int ID { get; set; }
    }

    public class CustomMock : ICustomMock
    {
        public CustomMock()
        {
            ID = 1;
        }

        public int ID { get; set; }
    }
}
