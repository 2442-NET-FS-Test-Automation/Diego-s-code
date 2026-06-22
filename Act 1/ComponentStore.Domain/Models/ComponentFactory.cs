 namespace ComponentStore.Domain;

class ComponentFactory
{
    public static Component Create(ComponentKind componentKind, string serialNumber, string name, decimal price, uint stock, int vram = 0, SocketType socketType = SocketType.AM4, MemoryType memoryType = MemoryType.DDR5, int memoryCapacity = 0, string watts = "0", int storage = 0, double clockSpeed = 0)
    {
        switch (componentKind)
        {
            case ComponentKind.GraphicCard:
                return new GraphicCard(serialNumber, name, price, stock, vram);
            case ComponentKind.MotherBoard:
                return new MotherBoard(serialNumber, name, price, stock, socketType, memoryType);
            case ComponentKind.PowerSupply:
                return new PowerSupply(serialNumber, name, price, stock, watts);
            case ComponentKind.Processor:
                return new Processor(serialNumber, name, price, stock, clockSpeed, socketType);
            case ComponentKind.RAM:
                return new RAM(serialNumber, name, price, stock, memoryCapacity, memoryType);
            case ComponentKind.SSD:
                return new SSD(serialNumber, name, price, stock, storage);
            default: // No idea how you'd get here.
                throw new ComponentException($"Unknown item kind: {componentKind}");
        }
    }
}

