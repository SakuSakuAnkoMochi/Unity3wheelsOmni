using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject boll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     var clone=Instantiate(boll,transform.position+new Vector3(0,2,0),Quaternion.identity);
        //     clone.GetComponent<Rigidbody>().AddForce(new Vector3(1000*Mathf.Sin(Mathf.PI/180*transform.rotation.y),500,1000*Mathf.Cos(Mathf.PI/180*transform.rotation.y)));
        // }
    }
}
