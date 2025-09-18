using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewVolumeBlender : MonoBehaviour
{
    // ----- FIELDS ----- //
    public static ViewVolumeBlender Instance;

    private List<AViewVolume> _activeViewVolumes = new List<AViewVolume>();

    private Dictionary<AView, List<AViewVolume>> _volumesPerViews = new Dictionary<AView, List<AViewVolume>>();
    // ----- FIELDS ----- //

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void AddVolume(AViewVolume volume)
    {
        
        if (!_activeViewVolumes.Contains(volume))
        {
            _activeViewVolumes.Add(volume);
        }

        if (!_volumesPerViews.ContainsKey(volume.View))  
        {
            List<AViewVolume> newList = new List<AViewVolume>();
            _volumesPerViews[volume.View] = newList;

            volume.View.SetActive(true);
        }

        if (!_volumesPerViews[volume.View].Contains(volume))
        {
            _volumesPerViews[volume.View].Add(volume);
        }
    }

    public void RemoveVolume(AViewVolume volume)
    {
        if (_activeViewVolumes.Contains(volume))
        {
            _activeViewVolumes.Remove(volume);
        }

        if (!_volumesPerViews.ContainsKey(volume.View)) return;
        if (_volumesPerViews[volume.View] == null) return;

        if (_volumesPerViews[volume.View].Contains(volume))
        {
            _volumesPerViews[volume.View].Remove(volume);
        }

        if (_volumesPerViews[volume.View].Count == 0)
        {
            _volumesPerViews.Remove(volume.View);
            volume.View.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateVolumes();
    }

    public void UpdateVolumes()
    {
        List<AViewVolume> _activeVolumesInPriority = new List<AViewVolume>(_activeViewVolumes);

        foreach (AViewVolume volume in _activeViewVolumes)
        {
            AView view = volume.View;

            view.Weight = 0;
        }

        // Sort volumes by weight (equality -> sort by UID)
        _activeVolumesInPriority.Sort((a, b) =>
        {
            int result = a.Priority.CompareTo(b.Priority);
            if (result != 0)
                return result;
            return a.GetUID().CompareTo(b.GetUID());
        });

        foreach (AViewVolume sortedVolume in _activeVolumesInPriority)
        {
            float selfWeight = Mathf.Clamp01(sortedVolume.ComputeSelfWeight()); // COMPUTE SELF WEIGHT RETURN 1

            float remainingWeight = 1 - selfWeight;

            foreach (AViewVolume volume in _activeVolumesInPriority) // WEIGHT = 0 JUSTE AU DESSUS
            {
                AView view = volume.View;
                view.Weight *= remainingWeight; // 0 *= 0
            }

            sortedVolume.View.Weight += selfWeight; // 0 + 1 = 1 
            //Debug.Log($"Sorted volume {sortedVolume.gameObject.name} weight : {sortedVolume.View.Weight}"); 
        }
    }

    private void OnGUI()
    {
        int i = 0;
        foreach (AViewVolume volume in _activeViewVolumes)
        {
            GUI.Label(new Rect(10, 10 + 25 * i, 350, 20), $"Active volume : {volume.gameObject.name}");
            i++;
        }
    }
}
