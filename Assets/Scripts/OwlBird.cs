using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlBird : Bird
{
    [SerializeField]
    public float _explosionMagnitude;
    public bool _hasExploded = false;
    public float _castArea;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!_hasExploded)
        {
            // Ketika menabrak benda lain dan belum meledak, OwlBird akan meledak
            Explode();
            _hasExploded = true;
        }
    }

    void Explode()
    {
        // Mendapatkan semua benda yang terdapat di sekitar OwlBird 
        Collider2D[] allObjectAround = Physics2D.OverlapCircleAll(RigidBody.position, _castArea);
        foreach (Collider2D objectAround in allObjectAround)
        {
            if (objectAround.gameObject.CompareTag("Enemy") || objectAround.gameObject.CompareTag("Obstacle"))
            {
                // Hanya benda ber-tag Enemy dan Obstacle
                // Mendapatkan jarak benda dari OwlBird
                Vector2 direction = objectAround.GetComponent<Rigidbody2D>().position - RigidBody.position;
                
                // Menentukan besar gaya yang diterima berdasarkan jarak dari OwlBird
                float magnitudeForObject = _explosionMagnitude * direction.magnitude/_castArea;
                
                objectAround.GetComponent<Rigidbody2D>().AddForce(direction.normalized * magnitudeForObject);
                if (objectAround.gameObject.CompareTag("Enemy"))
                {
                    objectAround.GetComponent<Enemy>().InflictDamage(magnitudeForObject);
                }
            }
        }
        // Hancurkan OwlBird setelah meledak
        Destroy(Collider.gameObject);
    }
}
