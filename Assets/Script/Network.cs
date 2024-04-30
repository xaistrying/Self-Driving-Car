using System;

public class NeuralNetwork
{
    private Layer[] layers;
    public NeuralNetwork(params int[] layerSizes)
    {
        layers = new Layer[layerSizes.Length - 1];
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new Layer(layerSizes[i], layerSizes[i + 1]);
        }
    }

    public double[] CalculatedOutputs(double[] inputs)
    {
        foreach (Layer layer in layers)
        {
            inputs = layer.CalculatedOutputs(inputs);
        }
        return inputs;
    }

    public static NeuralNetwork PassNetwork(NeuralNetwork origin)
    {
        NeuralNetwork network = new NeuralNetwork(5, 5, 2);
        for (int i = 0; i < origin.layers.Length; i++)
        {
            for (int nodeOut = 0; nodeOut < origin.layers[i].numNodesOut; nodeOut++)
            {
                for (int nodeIn = 0; nodeIn < origin.layers[i].numNodesIn; nodeIn++)
                {
                    network.layers[i].weights[nodeIn, nodeOut] = origin.layers[i].weights[nodeIn, nodeOut];
                }
            }

            for (int nodeOut = 0; nodeOut < origin.layers[i].numNodesOut; nodeOut++)
            {
                network.layers[i].biases[nodeOut] = origin.layers[i].biases[nodeOut];
            }
        }
        return network;
    }

    public static NeuralNetwork Mutate(NeuralNetwork network, double amount)
    {
        NeuralNetwork temp = NeuralNetwork.PassNetwork(network);
        foreach (Layer layer in temp.layers)
        {
            for (int nodeOut = 0; nodeOut < layer.numNodesOut; nodeOut++)
            {
                for (int nodeIn = 0; nodeIn < layer.numNodesIn; nodeIn++)
                {
                    layer.weights[nodeIn, nodeOut] = layer.weights[nodeIn, nodeOut]
                        + (UnityEngine.Random.Range(-1.0f, 1.0f) - layer.weights[nodeIn, nodeOut]) * amount;
                }
            }

            for (int nodeOut = 0; nodeOut < layer.numNodesOut; nodeOut++)
            {
                layer.biases[nodeOut] = layer.biases[nodeOut]
                    + (UnityEngine.Random.Range(-1.0f, 1.0f) - layer.biases[nodeOut]) * amount;
            }
        }
        return temp;
    }
}

public class Layer
{
    public int numNodesIn;
    public int numNodesOut;
    public double[,] weights;
    public double[] biases;

    public Layer(int numNodesIn, int numNodesOut)
    {
        this.numNodesIn = numNodesIn;
        this.numNodesOut = numNodesOut;

        weights = new double[numNodesIn, numNodesOut];
        biases = new double[numNodesOut];

        Randomize(this);
    }

    public void Randomize(Layer layer)
    {
        for (int nodeOut = 0; nodeOut < numNodesOut; nodeOut++)
        {
            for (int nodeIn = 0; nodeIn < numNodesIn; nodeIn++)
            {
                weights[nodeIn, nodeOut] = UnityEngine.Random.Range(-1.0f, 1.0f);
            }
        }

        for (int nodeOut = 0; nodeOut < numNodesOut; nodeOut++)
        {
            biases[nodeOut] = UnityEngine.Random.Range(-1.0f, 1.0f);
        }
    }

    public double[] CalculatedOutputs(double[] inputs)
    {
        double[] activations = new double[numNodesOut];

        for (int nodeOut = 0; nodeOut < numNodesOut; nodeOut++)
        {
            double weightedInput = biases[nodeOut];
            for (int nodeIn = 0; nodeIn < numNodesIn; nodeIn++)
            {
                weightedInput += inputs[nodeIn] * weights[nodeIn, nodeOut];
            }
            activations[nodeOut] = ActivationFunction(weightedInput);
        }
        return activations;
    }

    double ActivationFunction(double weightedInput)
    {
        double e2w = Math.Exp(2 * weightedInput);
        return (e2w - 1) / (e2w + 1);
    }
}