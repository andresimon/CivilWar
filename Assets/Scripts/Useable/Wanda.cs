using UnityEngine;
using System.Collections;

public class Wanda : MonoBehaviour, IUseable
{
    [SerializeField] private float pushForce;

    [SerializeField] private GameObject sorseryEffect;

    public GameObject textCanvas;

    #region IUseable implementation

    public void Use()
    {
        if (PlayerController.Instance.OnWanda) StartCoroutine(Catapult());
    }

    #endregion

    private IEnumerator Catapult()
    {
        PlayerController.Instance.AllowControl = false;
        sorseryEffect.SetActive(true);
        textCanvas.SetActive(false);

        yield return new WaitForSeconds(3);

        PlayerController.Instance.FlipPlayer(1);
        PlayerController.Instance.Anim.SetTrigger("Jump");

        PlayerController.Instance.MyRigidBody.AddForce(new Vector2(pushForce/2, pushForce), ForceMode2D.Impulse);
        PlayerController.Instance.AllowControl = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            PlayerController.Instance.OnWanda = true;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            PlayerController.Instance.OnWanda = false;
    }

}
