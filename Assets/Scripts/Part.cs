using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Part : MonoBehaviour
{
    public Car car;
    public int points;
    public List<Issue> issues = new List<Issue>();

    public float GetRepairTime()
    {
        return issues.Aggregate(0f, (repairTime, issue) => repairTime + Mathf.Max(issue.minimumRepairTime, issue.maximumRepairTime - GameManager.Instance.repairedCars * GameManager.Instance.difficultyIncreasePerRepairCar));

    }

    public Issue CurrentAction
    {
        get
        {
            if (issues.Count > 0)
                return issues[0];
            return null;
        }
    }

    public void Repair()
    {
        if (issues.Count > 0)
        {
            issues[0].SolveIssue(transform);
            issues.RemoveAt(0);
        }

    }
}
