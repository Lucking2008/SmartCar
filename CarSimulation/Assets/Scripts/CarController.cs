using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{

    public float speed = 100f;
    public float torque = 100f;
    public bool flagReal = false;
    public float rayLength = 2f;

    public Text textEpoch;
    public Text textQvalue;
    public Text textEpsilon;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Vector3 directionFront;

    private Vector3 directionLeft;
    private Vector3 directionRight;
    private Vector3 directionDiagonalLeft;
    private Vector3 directionDiagonalRight;

    private Transform tf;
    private Rigidbody2D rb;

    private Vector3 actualForce;
    private float torqueForce;

    private bool flagStop;
    private bool flagReset;
    private bool flagCollision;
    private bool flagLearning;

    INeuralNetwork QFunction;

    List<double> actualState;
    List<double> pastState;

    List<double> actualQvalue;
    List<double> pastQvalue;

    double actualAction;
    double pastAction;

    int actualActionIndex;
    int pastActionIndex;

    int maxEpochs;
    int actualEpoch;
    double epsilon;
    int displacement;

    struct Experience
    {
        public List<double> state;
        public double action;
        public List<double> nextState;
        public double reward;

        public Experience(List<double> state, double action, List<double> nextState, double reward)
        {
            this.state = state;
            this.action = action;
            this.nextState = nextState;
            this.reward = reward;
        }
    };

    List<double> actionList = new List<double>
    {
        -720d,
        -360d,
        -180d,
        -90d,
        0d,
        90d,
        180d,
        360d,
        720d
    };

    List<Experience> memory;
    int memoryPos;

    float distanceFront;
    float distanceLeft;
    float distanceRight;
    float distanceDiagonalLeft;
    float distanceDiagonalRight;

    Vector2 pointLeft;
    Vector2 pointRight;
    Vector2 pointDiagonalLeft;
    Vector2 pointDiagonalRight;

    Vector2 normalLeft;
    Vector2 normalRight;
    Vector2 normalDiagonalLeft;
    Vector2 normalDiagonalRight;

    // Use this for initialization
    void Start()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        startPosition = tf.position;
        startRotation = tf.rotation;

        directionFront = new Vector3(rayLength, 0f, 0f);
        directionLeft = new Vector3(0f, rayLength, 0f);
        directionRight = new Vector3(0f, -rayLength, 0f);
        directionDiagonalLeft = new Vector3(rayLength, rayLength, 0f);
        directionDiagonalRight = new Vector3(rayLength, -rayLength, 0f);


        directionFront = tf.rotation * directionFront;
        directionLeft = tf.rotation * directionLeft;
        directionRight = tf.rotation * directionRight;
        directionDiagonalLeft = tf.rotation * directionDiagonalLeft;
        directionDiagonalRight = tf.rotation * directionDiagonalRight;

        actualForce = Vector3.zero;
        torqueForce = 0f;

        flagStop = false;
        flagReset = false;
        flagReal = false;
        flagCollision = false;
        flagLearning = false;

        //Initialize QLearning parameters ------------------------------
        QFunction = new NeuralNetwork_Lucking();
        QFunction.Initialize(new List<int> { 9, 4, 4, actionList.Count });

        actualState = new List<double>(new double[9]);
        pastState = new List<double>(new double[9]);

        actualQvalue = new List<double>(actionList.Count);
        pastQvalue = new List<double>(actionList.Count);

        actualAction = -1;
        pastAction = -1;

        actualActionIndex = -1;
        pastActionIndex = -1;
        //--------------------------------------------------------------

        maxEpochs = 1000;
        actualEpoch = 1;
        epsilon = 1f;
        displacement = 0;

        memory = new List<Experience>();
        memoryPos = 0;

    }

    // Update is called once per frame
    void Update()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        actualForce = Vector3.zero;
        torqueForce = 0f;

        if (Input.GetKey("up"))
        {
            actualForce = directionFront.normalized * speed;
        }
        if (Input.GetKey("down"))
        {
            actualForce = (directionFront * -1f).normalized * speed;
        }
        if (Input.GetKey("left"))
        {
            torqueForce = torque;
        }
        if (Input.GetKey("right"))
        {
            torqueForce = -torque;
        }

        if (Input.GetKeyDown("s"))
            flagStop = true;

        if (Input.GetKeyDown("r"))
            flagReset = true;

        //AI
        RaycastHit2D hitFront = Physics2D.Raycast(tf.position, directionFront, directionFront.magnitude, LayerMask.GetMask("Walls"));
        RaycastHit2D hitLeft = Physics2D.Raycast(tf.position, directionLeft, directionLeft.magnitude, LayerMask.GetMask("Walls"));
        RaycastHit2D hitRight = Physics2D.Raycast(tf.position, directionRight, directionRight.magnitude, LayerMask.GetMask("Walls"));
        RaycastHit2D hitDiagonalLeft = Physics2D.Raycast(tf.position, directionDiagonalLeft, directionDiagonalLeft.magnitude, LayerMask.GetMask("Walls"));
        RaycastHit2D hitDiagonalRight = Physics2D.Raycast(tf.position, directionDiagonalRight, directionDiagonalRight.magnitude, LayerMask.GetMask("Walls"));

        distanceFront = 0f;
        distanceLeft = 0f;
        distanceRight = 0f;
        distanceDiagonalLeft = 0f;
        distanceDiagonalRight = 0f;

        pointLeft = tf.position;
        pointRight = tf.position;
        normalLeft = Vector2.zero;
        normalRight = Vector2.zero;

        if (hitFront.collider != null)
            distanceFront = hitFront.distance;
        if (hitLeft.collider != null)
        {
            pointLeft = hitLeft.point;
            normalLeft = hitLeft.normal;
            distanceLeft = hitLeft.distance;
        }
        if (hitRight.collider != null)
        {
            pointRight = hitRight.point;
            normalRight = hitRight.normal;
            distanceRight = hitRight.distance;
        }
        if (hitDiagonalLeft.collider != null)
        {
            pointDiagonalLeft = hitDiagonalLeft.point;
            normalDiagonalLeft = hitDiagonalLeft.normal;
            distanceDiagonalLeft = hitDiagonalLeft.distance;
        }
        if (hitDiagonalRight.collider != null)
        {
            pointDiagonalRight = hitDiagonalRight.point;
            normalDiagonalRight = hitDiagonalRight.normal;
            distanceDiagonalRight = hitDiagonalRight.distance;
        }

        float angleLeft = Vector2.Angle((Vector2)tf.position - pointLeft, normalLeft);
        if (normalLeft == Vector2.zero)
            angleLeft = 0f;
        float angleRight = Vector2.Angle((Vector2)tf.position - pointRight, normalRight);
        if (normalRight == Vector2.zero)
            angleRight = 0f;

        float angleDiagonalLeft = Vector2.Angle((Vector2)tf.position - pointDiagonalLeft, normalDiagonalLeft);
        if (normalDiagonalLeft == Vector2.zero)
            angleDiagonalLeft = 0f;
        float angleDiagonalRight = Vector2.Angle((Vector2)tf.position - pointDiagonalRight, normalDiagonalRight);
        if (normalDiagonalRight == Vector2.zero)
            angleDiagonalRight = 0f;

        /*
        Vector3 center = (hitLeft.point - hitRight.point) * 0.5f;
        torqueForce = (Mathf.Pow(1.4f, (center - tf.position).magnitude) * Mathf.Pow(10f, (1f / distanceFront))) - Mathf.Pow(5f, (1f / distanceDiagonalLeft)) + Mathf.Pow(5f, (1f / distanceDiagonalRight));
        */

        actualForce = directionFront.normalized * speed;


        //Q-Learning
        actualState[0] = distanceFront / 10d;
        actualState[1] = distanceLeft / 10d;
        actualState[2] = angleLeft / 180d;
        actualState[3] = distanceRight / 10d;
        actualState[4] = angleRight / 180d;
        actualState[5] = distanceDiagonalLeft / 10d;
        actualState[6] = angleDiagonalLeft / 180d;
        actualState[7] = distanceDiagonalRight / 10d;
        actualState[8] = angleDiagonalRight / 180d;

        actualQvalue = QFunction.Predict(actualState);

        if (actualEpoch <= maxEpochs)
        {
            if (pastActionIndex != -1)
            {
                
                double reward = getReward();
                /*
                actualQvalue = QFunction.Predict(actualState);
                actualAction = getBestAction(actualQvalue);

                List<double> y = new List<double>(pastQvalue);

                double update;
                if (!flagCollision)
                    update = reward + (0.9 * actualAction);
                else
                    update = reward;

                y[pastActionIndex] = QFunction.SigmoidFunction(update);
                QFunction.LearnIteration(new List<List<double>> { pastState }, new List<List<double>> { y });
                */

                //Experience replay
                Experience exp = new Experience(pastState, actionList[getBestActionIndex(actualQvalue)], actualState, reward);
                if (memory.Count <= 100)
                    memory.Add(exp);
                else
                {
                    if (memoryPos < 100)
                        memoryPos++;
                    else
                        memoryPos = 0;
                    memory[memoryPos] = exp;
                }

                //Random shuffle
                List<Experience> _memory = memory;
                for (int u = _memory.Count - 1; u > 0; u--)
                {
                    int v = Random.Range(0, u + 1);
                    Experience aux = _memory[u];
                    _memory[u] = _memory[v];
                    _memory[v] = aux;
                }

                List<Experience> miniBatch = _memory.GetRange(0, Mathf.Clamp(40, 0, _memory.Count));
                List<List<double>> input = new List<List<double>>();
                List<List<double>> output = new List<List<double>>();
                foreach (Experience e in miniBatch)
                {
                    List<double> oldQvalue = QFunction.Predict(e.state);
                    List<double> newQvalue = QFunction.Predict(e.nextState);

                    int actionIndex = getBestActionIndex(newQvalue);
                    double actionQValue = getBestAction(newQvalue);

                    double update;
                    if (!flagCollision)
                        update = getBestAction(oldQvalue) + 0.5 * (e.reward + (0.9 * actionQValue) - getBestAction(oldQvalue));
                    else
                        update = getBestAction(oldQvalue) + 0.5 * (e.reward - getBestAction(oldQvalue));

                    oldQvalue[actionIndex] = QFunction.SigmoidFunction(update);

                    input.Add(e.state);
                    output.Add(oldQvalue);
                }

                QFunction.LearnIteration(input, output);
            }

            pastQvalue = QFunction.Predict(actualState);
            if (Random.Range(0f, 1f) < epsilon)
                pastActionIndex = Random.Range(0, actionList.Count);
            else
                pastActionIndex = getBestActionIndex(pastQvalue);

            pastAction = getBestAction(pastQvalue);

            torqueForce = (float)actionList[pastActionIndex];

        }
        else
        {
            torqueForce = (float)actionList[getBestActionIndex(actualQvalue)];
        }

        pastState = new List<double>(actualState);
        pastQvalue = new List<double>(actualQvalue);
        //pastActionIndex = actualActionIndex;
        //pastAction = actualAction;

        displacement++;

        textEpoch.text = actualEpoch.ToString();
        if (pastActionIndex != -1)
            textQvalue.text = actionList[pastActionIndex].ToString() + " | " + pastAction.ToString();
        textEpsilon.text = epsilon.ToString();

    }

    void FixedUpdate()
    {
        if (flagReal)
        {
            rb.AddForce(actualForce * Time.deltaTime);
            rb.AddTorque(torqueForce * Time.deltaTime);
        }
        else
        {
            rb.velocity = actualForce * Time.deltaTime;
            rb.angularVelocity = torqueForce;
        }

        directionFront = tf.rotation * new Vector3(rayLength, 0f, 0f);
        directionLeft = tf.rotation * new Vector3(0f, rayLength, 0f);
        directionRight = tf.rotation * new Vector3(0f, -rayLength, 0f);
        directionDiagonalLeft = tf.rotation * new Vector3(rayLength, rayLength, 0f);
        directionDiagonalRight = tf.rotation * new Vector3(rayLength, -rayLength, 0f);

        if (flagStop)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
            flagStop = false;
        }

        if (flagReset)
        {
            tf.position = startPosition;
            tf.rotation = startRotation;
            flagReset = false;
        }

        Debug.DrawRay(tf.position, directionFront, Color.red);
        Debug.DrawRay(tf.position, directionLeft, Color.red);
        Debug.DrawRay(tf.position, directionRight, Color.red);
        Debug.DrawRay(tf.position, directionDiagonalLeft, Color.red);
        Debug.DrawRay(tf.position, directionDiagonalRight, Color.red);

        if (normalLeft != Vector2.zero)
            Debug.DrawRay(pointLeft, normalLeft);
        if (normalRight != Vector2.zero)
            Debug.DrawRay(pointRight, normalRight);
    }

    void LateUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        flagStop = true;
        flagReset = true;
        flagCollision = true;
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            pastState = null;
            pastQvalue = null;
            pastActionIndex = -1;
            pastAction = -1;

            if (epsilon > 0.01d)
                epsilon -= (1d / maxEpochs);
            actualEpoch++;
        }

    }

    /*List<double> getNextState(List<double> state, double action)
    {

    }*/

    double getReward()
    {
        if (flagCollision)
        {
            int _displacement = displacement;
            displacement = 0;
            flagCollision = false;
            return -500;
            //return -500d;
        }/*
        else
        {
            return 30 - (distanceFront + distanceLeft + distanceRight + distanceDiagonalLeft + distanceDiagonalRight) + displacement;
        }*/

        float angleLeft = Vector2.Angle((Vector2)tf.position - pointLeft, normalLeft);
        if (normalLeft == Vector2.zero)
            angleLeft = 0f;
        float angleRight = Vector2.Angle((Vector2)tf.position - pointRight, normalRight);
        if (normalRight == Vector2.zero)
            angleRight = 0f;

        float angleDiagonalLeft = Vector2.Angle((Vector2)tf.position - pointDiagonalLeft, normalDiagonalLeft);
        if (normalDiagonalLeft == Vector2.zero)
            angleDiagonalLeft = 0f;
        float angleDiagonalRight = Vector2.Angle((Vector2)tf.position - pointDiagonalRight, normalDiagonalRight);
        if (normalDiagonalRight == Vector2.zero)
            angleDiagonalRight = 0f;

        Debug.DrawRay(pointLeft, (Vector2)tf.position - pointLeft);
        Debug.DrawRay(pointRight, (Vector2)tf.position - pointRight);
        Debug.DrawRay(pointDiagonalLeft, (Vector2)tf.position - pointDiagonalLeft);
        Debug.DrawRay(pointDiagonalRight, (Vector2)tf.position - pointDiagonalRight);

        float difference1 = Mathf.Abs(angleLeft - angleRight);
        float difference2 = Mathf.Abs(angleDiagonalLeft - angleDiagonalRight);

        Vector2 roadW = pointLeft - pointRight;
        Vector2 roadC = pointRight + (roadW * 0.5f);
        Vector2 roadD = (Vector2)tf.position - roadC;
        float distanceToCenter = roadD.magnitude;


        return
            50 - distanceFront * 5d - distanceLeft * 5d - distanceRight * 5d;

        return
            (1d / (difference1 * 100000f + 1d)) * 10d
            - (1d / (difference2 * 10000f + 1d)) * 10d
            - distanceToCenter * 50d;

        return
            (1d / (difference1 * 100000f + 1d)) * 10d
            - distanceToCenter * 50f
            - (distanceFront) * 5d;

        return
            (1d / (difference1 * 100000f + 1d)) * 15d
            + (1d / (Mathf.Abs(distanceLeft - distanceRight) + 1d)) * 10d
            + (1d / (Mathf.Abs(distanceDiagonalLeft - distanceDiagonalRight) + 1d)) * 10d
            - (distanceDiagonalLeft) * 5d
            - (distanceDiagonalRight) * 5d
            + (1d / (Mathf.Abs(distanceLeft - distanceRight) + 1d)) * 2d - (distanceFront) * 5d + displacement;


        return
            + (1d / (Mathf.Abs(distanceLeft - distanceRight) + 1d)) * 10d
            + (1d / (Mathf.Abs(distanceDiagonalLeft - distanceDiagonalRight) + 1d)) * 10d
            - (distanceFront) * 20d 
            - (distanceDiagonalLeft) * 5d
            - (distanceDiagonalRight) * 5d
            + displacement * 10d;

    }

    int getBestActionIndex(List<double> actions)
    {
        double action = actions[0];
        int posAction = 0;
        for (int i = 0; i < actions.Count; i++)
            if (actions[i] > action)
            {
                action = actions[i];
                posAction = i;
            }

        return posAction;
    }

    double getBestAction(List<double> actions)
    {
        double action = actions[0];
        for (int i = 0; i < actions.Count; i++)
            if (actions[i] > action)
                action = actions[i];

        return action;
    }

    double Normalize(double x)
    {
        double minVal = -360d;
        double maxVal = 360d;
        double z = (x - minVal) / (maxVal - minVal);
        return z;
    }

    double Normalize(double x, double minVal, double maxVal)
    {
        double z = (x - minVal) / (maxVal - minVal);
        return z;
    }

    double Denormalize(double z)
    {
        double minVal = -360d;
        double maxVal = 360d;
        double x = z * (maxVal - minVal) + minVal;
        return x;
    }
}

/*
                //Experience replay
                Experience exp = new Experience(pastState, Normalize(actionList[getBestActionIndex(actualQvalue)]), actualState, getReward());
                if (memory.Count <= 100)
                    memory.Add(exp);
                else
                {
                    if (memoryPos < 100)
                        memoryPos++;
                    else
                        memoryPos = 0;
                    memory[memoryPos] = exp;
                }

                //Random shuffle
                List<Experience> _memory = memory;
                for(int u = _memory.Count - 1; u > 0; u--)
                {
                    int v = Random.Range(0, u + 1);
                    Experience aux = _memory[u];
                    _memory[u] = _memory[v];
                    _memory[v] = aux;
                }
                
                List<Experience> miniBatch = _memory.GetRange(0, Mathf.Clamp(40, 0, _memory.Count));
                List<List<double>> input = new List<List<double>>();
                List<List<double>> output = new List<List<double>>();
                foreach (Experience e in miniBatch)
                {
                    List<double> oldQvalue = QFunction.Predict(e.state);
                    List<double> newQvalue = QFunction.Predict(e.nextState);

                    int actionIndex = getBestActionIndex(newQvalue);
                    double action = actionList[actionIndex];

                    double update;
                    if (!flagCollision)
                        update = Normalize(actionList[getBestActionIndex(oldQvalue)]) + 0.1 * (e.reward + (0.7 * Normalize(action) - Normalize(actionList[getBestActionIndex(oldQvalue)])));
                    else
                        update = Normalize(actionList[getBestActionIndex(oldQvalue)]) + 0.1 * (e.reward - Normalize(actionList[getBestActionIndex(oldQvalue)]));

                    newQvalue[actionIndex] = QFunction.SigmoidFunction(update);

                    input.Add(e.state);
                    output.Add(newQvalue);
                }
                QFunction.LearnIteration(input, output);
                */
