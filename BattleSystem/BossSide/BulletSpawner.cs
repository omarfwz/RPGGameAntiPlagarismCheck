using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] GameObject pf_bullet;
    [SerializeField] float speed;
    [SerializeField] string type;
    [SerializeField] float cooldown;
    [SerializeField] float initial_cd;
    [SerializeField] float lifespan;
    [SerializeField] int amount;
    [SerializeField] float rotationChange;
    [SerializeField] float extRotationChange;
    [SerializeField] bool pointToPlayer; //this is a work in progress!

    private float transf_rotation;
    private float ex_rotation;
    private Transform transf;
    private float timer;
    private float curRotation;
    private bool called;
    private Transform plr; //WIP
    // Start is called before the first frame update
    void Start()
    {
        plr = playerControl.instance.rBody.gameObject.transform;
        if (type == "Circle")
        {
            rotationChange = 360 / amount;
        }

        transf = gameObject.GetComponent<Transform>();
        transf_rotation = transf.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= cooldown || (!called && timer >= initial_cd))
        {
            called = true;
            for (int i = 0; i < amount; i++)
            {
                curRotation += rotationChange;

                var tBullet = Instantiate(pf_bullet, transf.position, transf.rotation, transf);
                tBullet.GetComponent<BulletScript>().lifespan = this.lifespan;
                Rigidbody2D rb_tBullet = tBullet.GetComponent<Rigidbody2D>();
                /*
                if (pointToPlayer)
                {
                    Vector3 pos = transform.InverseTransformPoint(plr.position);
                    float pos_angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - 90;
                    transform.Rotate(0, 0, pos_angle);
                    transf_rotation = transf.rotation.eulerAngles.z;
                }
                */
                float rot = ex_rotation + curRotation + transf_rotation;
                rb_tBullet.velocity = new Vector2(speed * Mathf.Cos(rot * Mathf.PI / 180), speed * Mathf.Sin(rot * Mathf.PI / 180));
                rb_tBullet.SetRotation(rot);

            }
            ex_rotation += extRotationChange;
            curRotation = 0;
            timer = 0;
            if(type == "Single")
            {
                gameObject.SetActive(false);
            }
        }
    }
}
