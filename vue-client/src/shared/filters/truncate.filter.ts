export const truncateFilter = function(text: string, maxLength: number) {
  return text.slice(0, maxLength) + (maxLength < text.length ? '...' : '');
}
