
using DataAccess;
using Shared.Models;

namespace Business
{
    public class Service : IService
    {
        private IRepository _repository;

        public Service(IRepository repository)
        {
            _repository = repository;
        }

      
        public List<BOM> GetAllBOMs()
        {
            var resultList = new List<BOM>();
            var boms = _repository.GetBOMs().ToList();
            if (boms.Any())
            {
                foreach (var bom in boms)
                {
                    var res = new BOM()
                    {
                        Id = bom.Id,
                        ComponentName = bom.ComponentName,
                        ParentName = bom.ParentName,
                        Quantity = bom.Quantity
                    };
                    resultList.Add(res);
                }
            }
            return resultList;
        }

        public List<PART> GetAllParts()
        {
            var resultList = new List<PART>();
            var parts = _repository.GetParts().ToList();    
            if (parts.Any())
            {
                foreach (var part in parts)
                {
                    var res = new PART()
                    {
                        Id= part.Id,
                        Name = part.Name,
                        Material = part.Material,
                        Item = part.Item,
                        PartNumber = part.PartNumber,
                        Title = part.Title,
                        Type = part.Type
                    };
                    resultList.Add(res);
                }
            }
            return resultList;
        }

        public PART GetPART(int id)
        {
            var res = _repository.GetPART(id);
            if (res != null)
            {
                var part = new PART()
                {
                    Id = res.Id,
                    Name = res.Name,
                    Material = res.Material,
                    Item = res.Item,
                    PartNumber = res.PartNumber,
                    Title = res.Title,
                    Type = res.Type
                };

                return part;
            }

            return null;
        }

        public BOM GetBOM(int id)
        {
            var res = _repository.GetBOM(id);
            if (res != null)
            {
                var bom = new BOM()
                {
                   Id = res.Id,
                   ComponentName = res.ComponentName,
                   ParentName = res.ParentName,
                   Quantity = res.Quantity,
                   PartFkId = res.PartFkId

                };

                return bom;
            }

            return null;
        }

        public PART GetParentPart(int id)
        {
            var parent = _repository.GetParentPart(id);

            if (parent != null)
            {
                var part = new PART()
                {
                    Id = parent.Id,
                    Name = parent.Name,
                    Material = parent.Material,
                    Item = parent.Item,
                    PartNumber = parent.PartNumber,
                    Title = parent.Title,
                    Type = parent.Type
                };

                return part;
            }

            return null;

        }
        public List<PART> GetChildParts(int id)
        {
            var resultList = new List<PART>();
            var results = _repository.GetChildParts(id);

            if (results.Any())
            {
                foreach (var part in results)
                {
                    var res = new PART()
                    {
                        Id = part.Id,
                        Name = part.Name,
                        Material = part.Material,
                        Item = part.Item,
                        PartNumber = part.PartNumber,
                        Title = part.Title,
                        Type = part.Type
                    };
                    resultList.Add(res);
                }
            }

            return resultList;
        }

        public List<DetailedPART> GetDetailedParts()
        {
            var resultList = new List<DetailedPART>();
            var parts = _repository.GetPartsWithDetails();
            if (parts.Any())
            {
                foreach (var part in parts)
                {
                    var res = new DetailedPART()
                    {
                        Id = part.Id,
                        Guid = Guid.NewGuid().ToString(),
                        ParentName = part.ParentName ?? "",
                        ComponentName = part.ComponentName ?? "",
                        PartNumber = part.PartNumber ?? "",
                        Title = part.Title ?? "",
                        Quantity = part.Quantity,
                        Type = part.Type ?? "",
                        Item = part.Item ?? "",
                        Material = part.Material ?? "",
                    };
                    resultList.Add(res);
                }
            }
            return resultList;
        }

        public List<DetailedPART> GetComponentChildrenDetailedParts(int id)
        {
            var resultList = new List<DetailedPART>();
            var parts = _repository.GetComponentChildPartsWithDetails(id);
            if (parts.Any())
            {
                foreach (var part in parts)
                {
                    var res = new DetailedPART()
                    {
                        Id = part.Id,
                        Guid = Guid.NewGuid().ToString(),
                        ParentName = part.ParentName,
                        ComponentName = part.ComponentName,
                        PartNumber = part.PartNumber ?? "",
                        Title = part.Title ?? "",
                        Quantity = part.Quantity,
                        Type = part.Type ?? "",
                        Item = part.Item ?? "",
                        Material = part.Material ?? "",
                    };
                    resultList.Add(res);
                }
            }
            return resultList;
        }

        public Node GetTree(int id = 0)
        {
            
            var ancestor = (id == 0) ? 
                _repository.GetOrphanParts().FirstOrDefault() : 
                _repository.GetPartWithDetails(id);

            if (ancestor != null)
            {
                var root = new Node()
                {
                    Id = ancestor.Id,
                    Guid = Guid.NewGuid().ToString(),
                    Name = ancestor.ComponentName,
                    Item = ancestor?.Item,
                    Material = ancestor?.Material,
                    PartNumber = ancestor?.PartNumber,
                    Title = ancestor?.Title,
                    Type = ancestor?.Type,
                    ParentName = ancestor?.ParentName,
                    Quantity = ancestor?.Quantity,
                    Children = new List<Node>()
                };

                root.Children = _repository.GetComponentChildPartsWithDetails(ancestor.Id)
                    .Select(x => new Node()
                    {
                        Id = x.Id,
                        Guid = Guid.NewGuid().ToString(),
                        Name = x.ComponentName,
                        Item = x?.Item,
                        Material = x?.Material,
                        PartNumber = x?.PartNumber,
                        Title = x?.Title,
                        Type = x?.Type,
                        ParentName = x?.ParentName,
                        Quantity = x?.Quantity,
                        Children = new List<Node>()
                    }).ToList();

                Queue<Node> q = new Queue<Node>();
                q.Enqueue(root);

                HashSet<int> visited = new HashSet<int>(); // Track visited nodes

                while (q.Count != 0)
                {
                    int n = q.Count;

                    while (n > 0)
                    {
                        Node currentNode = q.Peek();
                        q.Dequeue();

                        // Check if the current node has been visited
                        if (!visited.Contains(currentNode.Id))
                        {
                            visited.Add(currentNode.Id);
                            Console.Write(currentNode.Id + " ");

                            for (int i = 0; i < currentNode.Children.Count; i++)
                            {
                                currentNode.Children[i].Children = _repository.GetComponentChildPartsWithDetails(currentNode.Children[i].Id)
                                    .Select(x => new Node()
                                    {
                                        Id = x.Id,
                                        Guid = Guid.NewGuid().ToString(),
                                        Name = x.ComponentName,
                                        Item = x?.Item,
                                        Material = x?.Material,
                                        PartNumber = x?.PartNumber,
                                        Title = x?.Title,
                                        Type = x?.Type,
                                        ParentName = x?.ParentName,
                                        Quantity = x?.Quantity,
                                        Children = new List<Node>()
                                    }).ToList();
                                q.Enqueue(currentNode.Children[i]);
                            }
                        }

                        n--;
                    }
                }

                return root;
            }

            return null;
        }



    }
}
