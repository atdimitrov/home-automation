namespace HomeAutomation.Shared
{
    public class Heater
    {
        public Heater(int id, State state, bool primary)
        {
            this.Id = id;
            this.State = state;
            this.Primary = primary;
        }

        public int Id { get; }

        public State State { get; }

        public bool Primary { get; }
    }
}
