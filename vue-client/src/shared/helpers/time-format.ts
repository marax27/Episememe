
export function formatDate(date: Date): string {
  const month = '' + (date.getMonth() + 1),
        day = '' + date.getDate(),
        year = date.getFullYear();

  return [
    year,
    month.length < 2 ? ('0' + month) : month,
    day.length < 2 ? ('0' + day) : day
  ].join('-');
}

export function formatTime(date: Date): string {
  const hours = date.getHours().toString(),
        minutes = date.getMinutes().toString();

  return (hours.length < 2 ? ('0' + hours) : hours)
       + ':'
       + (minutes.length < 2 ? ('0' + minutes) : minutes);
}
