export class CommonService {
  // API url
  public apiUrl = 'https://localhost:7294/';
  constructor() {}
}

export function enumToList(enumType: any) {
  return Object.keys(enumType).map((key) => ({
    value: enumType[key],
    title: key,
  }));
}
