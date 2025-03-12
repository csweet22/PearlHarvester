using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchParameters
{
    public Vector3 direction = Vector3.forward;
    public float speed = 5.0f;

    public LaunchParameters(Vector3 direction = new Vector3(), float speed = 5.0f)
    {
        this.direction = direction;
        this.speed = speed;
    }
}

public class LauncherComponent : MonoBehaviour
{
    [SerializeField] public ProjectileData projectileData;
    [SerializeField] protected Transform spawnLocation;
    [SerializeField] protected LayerMask spawnMask;
    [SerializeField] protected float spawnMaskRadius = 0.1f;

    public enum LauncherTeam
    {
        Player,
        Enemy
    }

    [SerializeField] protected LauncherTeam launcherTeam;

    public virtual bool Launch(LaunchParameters launchParameters = default)
    {
        if (launchParameters == null){
            launchParameters = new LaunchParameters();
        }

        Collider[] hitColliders = new Collider[1];
        Physics.OverlapSphereNonAlloc(spawnLocation.position, spawnMaskRadius, hitColliders, spawnMask);
        if (hitColliders[0] != null)
            return false;

        GameObject newProjectile =
            Instantiate(projectileData.projectilePrefab, spawnLocation.position, spawnLocation.rotation);
        ProjectileComponent projectileComponent = newProjectile.GetComponent<ProjectileComponent>();
        projectileComponent.Init(projectileData, launchParameters.direction * projectileData.baseSpeed, launcherTeam);
        return true;
    }
}