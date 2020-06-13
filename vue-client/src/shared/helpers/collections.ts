export function pushUnique<T>(collection: T[], newElements: T[]) {
  newElements.forEach(element => {
    if (collection.indexOf(element) === -1)
      collection.push(element);
  });
}

export function intersectionOf<T>(firstCollection: T[], otherCollection: T[]): T[] {
  return firstCollection
    .filter(elem => otherCollection.indexOf(elem) !== -1);
}
