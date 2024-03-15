using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerMovement pm;
    private PlayerAttack pa;
    private PlayerSummon ps;
    private PlayerReveal pr;
    public bool isPickedUp;

    public float amp;
    public float freq;

    public string id;
    Vector3 initPos;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            pm = player.GetComponent<PlayerMovement>();
            pa = player.GetComponent<PlayerAttack>();
            ps = player.GetComponent<PlayerSummon>();
            pr = player.GetComponent<PlayerReveal>();
            if (id == "dash"){
            pm.learnDash();
            }
            if (id == "attack"){
            pa.learnAttack();
            }
            if (id == "summon"){
            ps.learnSummon();
            }
            if (id == "reveal"){
            pr.learnReveal();
            }

            Destroy(gameObject);
        }else{
            transform.position = new Vector3(initPos.x,Mathf.Sin(Time.time + freq) * amp + initPos.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && !isPickedUp)
        {
            isPickedUp = true;
        }
    }
}
