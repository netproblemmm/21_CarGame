using Features.Inventory;
using Game;
using Profile;
using UI;
using UnityEngine;

internal class MainController: BaseController
{
    private readonly Transform _placeForUI;
    private readonly ProfilePlayer _profilePlayer;

    private MainMenuController _mainMenuController;
    private InventoryController _inventoryController;
    private SettingsMenuController _settingsMenuController;
    private GameController _gameController;

    public MainController(Transform placeForUI, ProfilePlayer profilePlayer)
    {
        _placeForUI = placeForUI;
        _profilePlayer = profilePlayer;

        profilePlayer.CurrentState.SubscribeOnChange(OnChangeGameState);
        OnChangeGameState(_profilePlayer.CurrentState.Value);
    }

    protected override void OnDispose()
    {
        _mainMenuController?.Dispose();
        _settingsMenuController?.Dispose();
        _gameController?.Dispose();
        _inventoryController.Dispose();
        _profilePlayer.CurrentState.UnSubscribeOnChange(OnChangeGameState);
    }

    private void OnChangeGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                _mainMenuController = new MainMenuController(_placeForUI, _profilePlayer);
                _settingsMenuController?.Dispose();
                _inventoryController.Dispose();
                _gameController?.Dispose();
                break;
            case GameState.Settings:
                _settingsMenuController = new SettingsMenuController(_placeForUI, _profilePlayer);
                _mainMenuController?.Dispose();
                _inventoryController.Dispose();
                _gameController?.Dispose();
                break;
            case GameState.Game:
                _gameController = new GameController(_profilePlayer);
                _settingsMenuController?.Dispose();
                _inventoryController.Dispose();
                _mainMenuController?.Dispose();
                break;
            case GameState.Inventory:
                _inventoryController = new InventoryController(_placeForUI, _profilePlayer.InventoryModel);
                _settingsMenuController?.Dispose();
                _gameController?.Dispose();
                _mainMenuController?.Dispose();
                break;
            default:
                _mainMenuController?.Dispose();
                _inventoryController.Dispose();
                _settingsMenuController?.Dispose();
                _gameController?.Dispose();
                break;
        }
    }
}

