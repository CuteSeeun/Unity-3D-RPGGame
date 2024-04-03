using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HJ;

public class Artifact : MonoBehaviour, IInteractable
{
    public bool _isLocked;
    [SerializeField] GameObject _interactorLight;
    [SerializeField] GameObject _LockedLight;
    [SerializeField] GameObject _effect;
    [SerializeField] int _sound;
    [SerializeField] List<GameObject> interactables;

    public void InteractableOn()
    {
        if (_isLocked == false)
        {
            _interactorLight.SetActive(true);
        }
        else
        {
            _LockedLight.SetActive(true);
        }
    }

    public void InteractableOff()
    {
        if (_isLocked == false)
        {
            _interactorLight.SetActive(false);
        }
        else
        {
            _LockedLight.SetActive(false);
        }
    }

    public void LockOff()
    {
        _isLocked = false;
        _LockedLight.SetActive(true);
    }

    public void Interaction(GameObject interactor)
    {
        if (_isLocked == false)
        {
            //interactor.GetComponent<PlayerControllerForTest>().PotionFull();
            interactor.GetComponent<PlayerController>().PotionFull();

            foreach (var item in interactables)
            {
                item.GetComponent<IInteractable>().Interaction(gameObject);
            }

            StartCoroutine(Effect(_effect, _sound, 1));

            Destroy(gameObject);
        }
    }

    IEnumerator Effect(GameObject effect, int soundNum, float delay)
    {
        GameObject effectInstanse = Instantiate(effect, transform.position, transform.rotation);
        SFX_Manager.Instance.VFX(soundNum);

        yield return new WaitForSeconds(delay);
        Destroy(effectInstanse);
    }
}
