using BaseLibrary.protocol;

namespace ObstacleMod {
    public abstract class AObtacleFactory : AFactory<IObtacle, IObtacle> {
        protected Factories<IObtacle, IObtacle> comandsFactory = new Factories<IObtacle, IObtacle>();
    }
}
