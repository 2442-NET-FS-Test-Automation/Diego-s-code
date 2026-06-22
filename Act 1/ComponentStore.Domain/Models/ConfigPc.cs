using System.ComponentModel.Design;
using Serilog.Core.Enrichers;

namespace ComponentStore.Domain;

public class ConfigPc
{
    public string Name {get; set;}

    private Stack<Component> _component = new Stack<Component>();

    public ConfigPc(string name)
    {
        Name = name;
    }


    public void AddComponent(Component newComponent)
    {

        MotherBoard CurrentMB = null;
        bool HasProcessor = false;
        bool HasMb = false; 

            foreach(var piece in _component)
        {
            
            if(piece.SerialNumber == newComponent.SerialNumber)
            {
                Console.WriteLine("\n This component was added before!");
                return;
            }

            if(piece is MotherBoard mb)
            {
                CurrentMB = mb;
                HasMb = true;
            }
            else if (piece is Processor)
            {
                HasProcessor = true;
            }
        }

        if(newComponent is Processor && HasProcessor)
        {
            Console.WriteLine("This Configuration has a processor already!");
            return;
        }

        if(newComponent is MotherBoard && HasMb)
        {
            Console.WriteLine("This configuration has a Mother Board already!");
            return;
        }

        if(newComponent is Processor proc && CurrentMB != null)
        {
            if(proc.Socket != CurrentMB.Socket)
            {
                /*
                Console.WriteLine("The Sockets aren't compatible");
                Console.WriteLine($"Processor Socket: {proc.Socket}, Mother Board Socket: {CurrentMB.Socket}");
                return;
                */

                throw new ComponentException($"The Sockets aren't compatible. \nProcessor Socket: {proc.Socket}, Mother Board Socket: {CurrentMB.Socket} ");
            }
        }
        else if(newComponent is RAM ram && CurrentMB != null)
        {
            if(ram.Memory != CurrentMB.Memory)
            {
                /*
                Console.WriteLine("The Memory types aren't compatible");
                Console.WriteLine($"Ram Memory type: {ram.Memory}, Mother Board Memory type: {CurrentMB.Memory}");
                return;
                */

                throw new ComponentException($"The Memory types aren't compatible. \nRam Memory type: {ram.Memory}, Mother Board Memory type: {CurrentMB.Memory}");
            }
        }
        else if (newComponent is MotherBoard newMB)
        {
            foreach(var piece in _component)
            {
                if(piece is Processor existingProc && existingProc.Socket != newMB.Socket)
                {
                    /*
                    Console.WriteLine("The Sockets aren't compatible");
                    Console.WriteLine($"Processor Socket: {existingProc.Socket}, Mother Board Socket: {newMB.Socket}");
                    return;
                    */

                    throw new ComponentException($"The Sockets aren't compatible. \nProcessor Socket: {existingProc.Socket}, Mother Board Socket: {newMB.Socket} ");
                }

                if(piece is RAM existingRam && existingRam.Memory != newMB.Memory)
                {
                    /*
                    Console.WriteLine("The Sockets aren't compatible");
                    Console.WriteLine($"Processor Socket: {existingRam.Memory}, Mother Board Socket: {newMB.Memory}");
                    return;
                    */

                    throw new ComponentException($"The Sockets aren't compatible. \nProcessor Socket: {existingRam.Memory}, Mother Board Socket: {newMB.Memory} ");
                }
            }   
        }

        _component.Push(newComponent);
        Console.WriteLine($"{newComponent.Name} was added succesfully");
        
    }

        public void ListComponents()
    {
    
        if(_component.Count == 0)
        {
            Console.WriteLine("Your config has no components, add some!");
            return;
        }

        Console.WriteLine($"=== Components in {Name} ===");

        int i = 1;
        decimal totalPrice = 0;

        foreach (var item in _component)
        {
            Console.WriteLine($"{i}.  {item.Name} | Price: {item.Price} | S/N: {item.SerialNumber}");
            totalPrice += item.Price;
            i++;
        }

        Console.WriteLine($"=== Total price: {totalPrice}");
    }


    public void RemoveComponent(int choice)
    {
        if(_component.Count == 0)
        {
            Console.WriteLine("This config is empty");
            return;
        }
        if(choice < 1 || choice > _component.Count)
        {
            throw new ComponentNotFoundException(choice);
        }

        Stack<Component> tempStack = new Stack<Component>();
        Component removedPiece = null;
        int currentIndex = 1;


        while (_component.Count > 0)
        {
            var piece = _component.Pop();

            if(currentIndex == choice)
            {
                removedPiece = piece;
            }
            else
            {
                tempStack.Push(piece);
            }
            currentIndex++;
        }

        while (tempStack.Count > 0)
        {
            _component.Push(tempStack.Pop());
        }

        if(removedPiece != null)
        {
            Console.WriteLine("The piece was removed succesfully!");
            Console.WriteLine($"{removedPiece.Name} was removed from the config");
        }
    }

    public bool HasComponents()
    {
        return _component.Count > 0;
    }

}