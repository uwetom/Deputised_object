void onPing(const OscMessage& msg)
{
  Serial.print(msg.remoteIP());
  Serial.print(" ");
  Serial.print(msg.remotePort());
  Serial.print(" ");
  Serial.print(msg.size());
  Serial.print(" ");
  Serial.print(msg.address());
  Serial.print(" ");
  Serial.print(msg.arg<int>(0));
  Serial.print(" ");
  Serial.print(msg.arg<float>(1));
  Serial.print(" ");
  Serial.print(msg.arg<String>(2));
  Serial.println();
}
