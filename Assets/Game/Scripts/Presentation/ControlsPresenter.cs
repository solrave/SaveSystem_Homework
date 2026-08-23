using System;

namespace Game.Gameplay
{
    public class ControlsPresenter : IControlsPresenter
    {
        private SaveManager _saveManager;

        public ControlsPresenter(SaveManager saveManager)
        {
            _saveManager = saveManager;
        }

        public void Save(Action<bool, int> callback)
        {
          
        }

        public void Load(string version, Action<bool, int> callback)
        {
            throw new NotImplementedException();
        }
    }
}