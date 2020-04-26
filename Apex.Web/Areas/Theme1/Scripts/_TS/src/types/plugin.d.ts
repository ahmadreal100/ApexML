type alertType = "warning" | "error" | "success" | "info" | "question";
interface ISwal {
  fire: (options: ISwalOption) => void;
  mixin: (options: ISwalOption) => ISwal;
  stopTimer?: any;
  resumeTimer?: any;
}
interface ISwalOption {
  toast?: boolean;
  icon?: alertType;
  title?: string;
  html?: string;
  text?: string;
  timer?: number;
  timerProgressBar?: boolean;
  showConfirmButton?: boolean;
  position?:
    | "top"
    | "top-start"
    | "top-end"
    | "center"
    | "center-start"
    | "center-end"
    | "bottom"
    | "bottom-start"
    | "bottom-end";
  onOpen?: (toast?: any) => void;
}
