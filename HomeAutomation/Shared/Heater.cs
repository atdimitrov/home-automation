namespace HomeAutomation.Shared
{
    public class Heater
    {
        public Heater(int id, State state)
        {
            this.Id = id;
            this.State = state;
        }

        public int Id { get; }

        public State State { get; }
    }
}
