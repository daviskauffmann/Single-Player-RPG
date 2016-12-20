using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class StatusBars : MonoBehaviour
    {
        [SerializeField]GameObject _characterHealthBar = null;
        [SerializeField]GameObject _characterEnergyBar = null;
        [SerializeField]GameObject _characterManaBar = null;
        [SerializeField]GameObject _targetHealthBar = null;
        [SerializeField]GameObject _targetEnergyBar = null;
        [SerializeField]GameObject _targetManaBar = null;

        void Update()
        {
            _characterHealthBar.transform.localScale = new Vector3(GameManager.character.currentHealth / GameManager.character.maxHealth, 1, 1);
            _characterEnergyBar.transform.localScale = new Vector3(GameManager.character.currentEnergy / GameManager.character.maxEnergy, 1, 1);
            _characterManaBar.transform.localScale = new Vector3(GameManager.character.currentMana / GameManager.character.maxMana, 1, 1);

            if (GameManager.character.selectedTarget != null)
            {
                _targetHealthBar.transform.localScale = new Vector3(GameManager.character.selectedTarget.GetComponent<Character>().currentHealth / GameManager.character.selectedTarget.GetComponent<Character>().maxHealth, 1, 1);
                _targetEnergyBar.transform.localScale = new Vector3(GameManager.character.selectedTarget.GetComponent<Character>().currentEnergy / GameManager.character.selectedTarget.GetComponent<Character>().maxEnergy, 1, 1);
                _targetManaBar.transform.localScale = new Vector3(GameManager.character.selectedTarget.GetComponent<Character>().currentMana / GameManager.character.selectedTarget.GetComponent<Character>().maxEnergy, 1, 1);

                _targetHealthBar.SetActive(true);
                _targetEnergyBar.SetActive(true);
                _targetManaBar.SetActive(true);
            }
            else
            {
                _targetHealthBar.SetActive(false);
                _targetEnergyBar.SetActive(false);
                _targetManaBar.SetActive(false);
            }
        }
    }
}