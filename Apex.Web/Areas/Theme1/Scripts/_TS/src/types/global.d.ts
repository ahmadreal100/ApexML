export {};
declare global {
  var Str: IStr;
  var Swal: ISwal;
  interface JQueryStatic {}
  interface JQuery {
    owlCarousel: (o?: any) => any;
    rules: (o1?: any, o2?: any) => any;
  }
  interface HTMLElement {
    reset: () => void;
  }

  interface String {
    ft: (this: string, ...items: Array<string>) => string;
    capitalize: (this: string) => string;
  }

  interface Number {
    thousand: (this: number, decimal?: number) => string;
  }
}
