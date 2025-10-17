using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

[RequireComponent(typeof(ARTrackedImageManager))]
public class MarkerPrefabSpawner : MonoBehaviour
{
    public GameObject prefab; // seu modelo ou objeto
    private ARTrackedImageManager trackedImageManager;
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var addedImage in args.added)
        {
            SpawnOrUpdatePrefab(addedImage);
        }

        foreach (var updatedImage in args.updated)
        {
            SpawnOrUpdatePrefab(updatedImage);
        }

        foreach (var removedImage in args.removed)
        {
            if (spawnedPrefabs.ContainsKey(removedImage.referenceImage.name))
            {
                Destroy(spawnedPrefabs[removedImage.referenceImage.name]);
                spawnedPrefabs.Remove(removedImage.referenceImage.name);
            }
        }
    }

    void SpawnOrUpdatePrefab(ARTrackedImage trackedImage)
    {
        var name = trackedImage.referenceImage.name;

        if (!spawnedPrefabs.ContainsKey(name))
        {
            var newPrefab = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
            spawnedPrefabs.Add(name, newPrefab);
        }
        else
        {
            spawnedPrefabs[name].transform.position = trackedImage.transform.position;
            spawnedPrefabs[name].transform.rotation = trackedImage.transform.rotation;
        }
    }
}
