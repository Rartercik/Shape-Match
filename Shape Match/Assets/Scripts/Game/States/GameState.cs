namespace Game.States
{
    public abstract class GameState
    {
        public GameState(GameStatesHandler statesHandler)
        {
            StatesHandler = statesHandler;
        }

        protected GameStatesHandler StatesHandler { get; private set; }

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void ReportBeginProcess() { }

        public virtual void ReportVictory() { }

        public virtual void ReportDefeat() { }

        public virtual void ReportPreparationRequest() { }
    }
}
