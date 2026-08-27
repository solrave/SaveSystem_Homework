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
            (bool result, int version) = _saveManager.Save();
            callback?.Invoke(result,version);
        }

        public void Load(string version, Action<bool, int> callback)
        {
            if (int.TryParse(version, out var versionNumber))
            {
                bool result;
                (result, versionNumber) = _saveManager.Load(versionNumber);
                callback?.Invoke(result, versionNumber );
            }
            
        }
    }
}