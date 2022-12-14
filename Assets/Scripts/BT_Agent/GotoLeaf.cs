using UnityEngine;
using UnityEngine.AI;


// A leaf node that takes the agent to a given waypoint
public class GotoLeaf : Node
{
    GameObject waypoint;
    private enum ActionState { IDLE, WORKING };
    private ActionState actionState = ActionState.IDLE;
    NavMeshAgent navMeshAgent;
    GameObject agent;
    BT_AgentController agentController;


    public GotoLeaf(string name, GameObject waypoint, NavMeshAgent nma, GameObject agent, Node parent) : base(name, parent)
    {
        this.waypoint = waypoint;
        this.navMeshAgent = nma;
        this.agent = agent;
        agentController = agent.GetComponent<BT_AgentController>();
    }


    /// <summary>
    /// Makes the agent navigate to a given destination
    /// </summary>
    /// <param name="destinatoin">A waypoint or position to move to</param>
    /// <returns>The BT-node status</returns>
    private Node.Status GotoLocation(Vector3 destinatoin)
    {
        if (actionState == ActionState.IDLE)
        {
            navMeshAgent.SetDestination(destinatoin);
            actionState = ActionState.WORKING;
        }
        else if (Vector3.Distance(navMeshAgent.pathEndPosition, destinatoin) >= 2)
        {
            actionState = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (Vector3.Distance(destinatoin, agent.transform.position) < 2)
        {
            actionState = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }
    
    
    public override Status Process()
    {
        if(name == "Goto Work" || name == "Via Path 1" || name == "Via Path 2" || name == "Via Path 3" || name == "Goto Home")
            agentController.traveling = true;
        else
            agentController.traveling = false;

        BT_AgentController.actionStatusText.text = name;
        return GotoLocation(waypoint.transform.position);
    }
}
