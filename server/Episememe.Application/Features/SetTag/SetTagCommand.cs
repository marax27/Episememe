using System;
using System.Collections.Generic;
using System.Linq;
using Episememe.Application.DataTransfer;
using MediatR;

namespace Episememe.Application.Features.SetTag
{
    public class SetTagCommand : IRequest
    {
        public string CurrentName { get; }
        public string NewName { get; }
        public string? NewDescription { get; }
        public IReadOnlyCollection<string> NewChildren { get; }
        public IReadOnlyCollection<string> NewParents { get; }

        public SetTagCommand(string currentName, string newName, string? description,
            IReadOnlyCollection<string> newChildren, IReadOnlyCollection<string> newParents)
        {
            CurrentName = currentName;
            NewName = newName;
            NewDescription = description;
            NewChildren = newChildren;
            NewParents = newParents;
        }

        public static SetTagCommand Create(string currentName, SetTagDto dto)
        {
            if (currentName == null)
                throw new ArgumentNullException(nameof(currentName));
            if (dto.Name == null)
                throw new ArgumentNullException(nameof(dto.Name));
            if (dto.Children == null)
                throw new ArgumentNullException(nameof(dto.Children));
            if (dto.Parents == null)
                throw new ArgumentNullException(nameof(dto.Parents));

            if (dto.Children.Intersect(dto.Parents).Any())
                throw new ArgumentException($"An item appears in both {nameof(dto.Children)} and {nameof(dto.Parents)}.");

            return new SetTagCommand(currentName, dto.Name, dto.Description,
                dto.Children.ToList().AsReadOnly(), dto.Parents.ToList().AsReadOnly());
        }
    }
}
