import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "enumToArray",
})
export class EnumToArray implements PipeTransform {
  transform(value: any, ...args: any[]) {
    return Object.keys(value)
      .filter((e) => !isNaN(+e))
      .map((o) => {
        return { index: +o, name: value[o] };
      });
  }
}
